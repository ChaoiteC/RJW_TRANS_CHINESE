import os
import xml.etree.ElementTree as ET

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

    # 插入新元素
    for original_elem, new_elem in elements_to_add:
        index = list(root).index(original_elem) + 1
        root.insert(index, new_elem)

    # 手动生成紧凑且正确格式的XML字符串
    xml_lines = ['<?xml version="1.0" encoding="utf-8"?>', '<LanguageData>']

    for elem in root:
        xml_lines.append(f"  <{elem.tag}>{elem.text}</{elem.tag}>")
        if elem.tag.endswith('.RMBLabel'):
            xml_lines.append("")  # 在RMBLabel标签后添加一个换行符

    xml_lines.append('</LanguageData>')

    # 写入文件，确保内容不被转义
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write("\n".join(xml_lines))

def process_all_files_in_directory(directory):
    for filename in os.listdir(directory):
        if filename.endswith(".xml"):
            file_path = os.path.join(directory, filename)
            process_xml_file(file_path)
            print(f"Processed {filename}")

if __name__ == "__main__":
    directory = os.path.dirname(os.path.abspath(__file__))  # 获取脚本所在目录
    process_all_files_in_directory(directory)
