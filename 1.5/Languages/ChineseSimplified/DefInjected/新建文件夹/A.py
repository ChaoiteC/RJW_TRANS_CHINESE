import os
import xml.etree.ElementTree as ET
from xml.dom import minidom

def prettify_xml(element):
    """Return a pretty-printed XML string for the Element."""
    rough_string = ET.tostring(element, encoding='utf-8', method='xml')
    parsed = minidom.parseString(rough_string)
    return parsed.toprettyxml(indent="  ", encoding='utf-8').decode('utf-8')

def process_xml_file(file_path):
    tree = ET.parse(file_path)
    root = tree.getroot()

    elements_to_add = []
    for elem in root:
        if elem.tag.endswith('.label'):
            parent_tag = elem.tag.split('.')[0]
            new_tag = f"{parent_tag}.modExtensions.0.RMBLabel"

            # 检查是否已经存在相应的RMBLabel标签
            if root.find(new_tag) is None:
                # 创建新的元素并存储，以便在循环结束后插入
                new_element = ET.Element(new_tag)
                new_element.text = elem.text
                elements_to_add.append((elem, new_element))

    # 插入新元素并保持格式
    for original_elem, new_elem in elements_to_add:
        index = list(root).index(original_elem) + 1
        root.insert(index, new_elem)

    # 生成美观的XML字符串
    pretty_xml_str = prettify_xml(root)

    # 手动处理空行插入
    lines = pretty_xml_str.splitlines()
    output_lines = []
    for i, line in enumerate(lines):
        output_lines.append(line)
        if line.strip().endswith('.label>'):
            output_lines.append('')  # 在 .label 标签之后插入空行
        if line.strip().endswith('.rulesStrings.0>'):
            output_lines.append('')  # 在 rulesStrings.0 标签之后插入空行

    # 写入文件，保持正确的格式和字符处理
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write('\n'.join(output_lines))

def process_all_files_in_directory(directory):
    for filename in os.listdir(directory):
        if filename.endswith(".xml"):
            file_path = os.path.join(directory, filename)
            process_xml_file(file_path)
            print(f"Processed {filename}")

if __name__ == "__main__":
    directory = os.path.dirname(os.path.abspath(__file__))  # 获取脚本所在目录
    process_all_files_in_directory(directory)
