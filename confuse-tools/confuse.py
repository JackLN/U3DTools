#! /usr/bin/env python
#coding=utf-8

import hashlib
import random
import os 

#������ı���/������
raw_name_list = []
                
#������ı���/������
new_name_list = []

#�����ѡ����ĸ��
alphabet = ["a", "b", "c", "d", "e", "f", "g",  
    "h", "i", "j", "k", "l", "m", "n", "o", "p", "q",  
    "r", "s", "t", "u", "v", "w", "x", "y", "z", 
] 

FUNC_NAME_FILE="func-name.txt"

def read_name_list():
    f = file(FUNC_NAME_FILE)
    while True:
        line = f.readline()
        if len(line)==0:
            break
        line=line.strip('\n')
        raw_name_list.append(line)
    f.close()

    print "Target Func Name = ",raw_name_list

#�����µı�����
def create_new_name() : 
    m = hashlib.md5() 
    #������������� 
    for raw_name in raw_name_list: 
        m.update(raw_name)
        #����һ��16λ���ִ� 
        temp_name = m.hexdigest()[0:16]
        #�Ϸ�����У��
        #ǿ������ĸ��Ϊ����/�������Ŀ�ͷ 
        if temp_name[0].isdigit(): 
            initial = random.choice(alphabet)
            temp_name = initial + temp_name 
            temp_name = temp_name[0:16] 
        #��������
        while(1): 
            if temp_name in new_name_list :
                initial = random.choice(alphabet)
                temp_name = initial + temp_name
                temp_name = temp_name[0:16] 
            else:
                new_name_list.append(temp_name)
                break

#�����ļ�
def confuse_file(path_filename):  
    file_content = "" 
    #���ļ����� 
    f = file(path_filename)
    # if no mode is specified, 'r'ead mode is assumed by default
    while True:
        line = f.readline() 
        if len(line) == 0: # Zero length indicates EOF
            break 
        #����
        name_index = 0
        for raw_name in raw_name_list: 
            the_new_name = new_name_list[name_index] 
            line = line.replace(raw_name, the_new_name) 
            name_index += 1 
        file_content += line
    f.close() 
    #��д�ļ�
    f = file(path_filename, 'w') 
    f.write(file_content)
    f.close() 
    
#������ǰĿ¼�µ�����.cs�ļ�    
def confuse_all(): 
    #��ȡ��ǰĿ¼
    dir = os.getcwd() 
    for root, dirs, filename in os.walk(dir):  
        for file in filename:  
            path_filename = os.path.join(root, file) 
            if path_filename.endswith('.cs'): 
                confuse_file(path_filename)  
                print "Confuse File: ", path_filename 

        
if __name__=="__main__": 
    read_name_list()
    create_new_name() 
    confuse_all() 
    #��ӡһ�»��������. 
    #������ı���������, ��ô�Ժ���Է�����, ��ԭ�ļ�
    print "Start Confuse ...." 
    for j in range(0, len(raw_name_list)) :
        print raw_name_list[j] , " --> " , new_name_list[j] 
    print "Confuse Complete !" 
