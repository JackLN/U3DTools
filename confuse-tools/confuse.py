#! /usr/bin/env python
#coding=utf-8

import hashlib
import random
import os 

#想混淆的变量/方法名
raw_name_list = []
                
#混淆后的变量/方法名
new_name_list = []

#随机可选的字母表
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

#生成新的变量名
def create_new_name() : 
    m = hashlib.md5() 
    #生成随机变量名 
    for raw_name in raw_name_list: 
        m.update(raw_name)
        #生成一个16位的字串 
        temp_name = m.hexdigest()[0:16]
        #合法名称校验
        #强制以字母作为变量/方法名的开头 
        if temp_name[0].isdigit(): 
            initial = random.choice(alphabet)
            temp_name = initial + temp_name 
            temp_name = temp_name[0:16] 
        #不能重名
        while(1): 
            if temp_name in new_name_list :
                initial = random.choice(alphabet)
                temp_name = initial + temp_name
                temp_name = temp_name[0:16] 
            else:
                new_name_list.append(temp_name)
                break

#混淆文件
def confuse_file(path_filename):  
    file_content = "" 
    #读文件内容 
    f = file(path_filename)
    # if no mode is specified, 'r'ead mode is assumed by default
    while True:
        line = f.readline() 
        if len(line) == 0: # Zero length indicates EOF
            break 
        #混淆
        name_index = 0
        for raw_name in raw_name_list: 
            the_new_name = new_name_list[name_index] 
            line = line.replace(raw_name, the_new_name) 
            name_index += 1 
        file_content += line
    f.close() 
    #重写文件
    f = file(path_filename, 'w') 
    f.write(file_content)
    f.close() 
    
#遍历当前目录下的所有.cs文件    
def confuse_all(): 
    #获取当前目录
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
    #打印一下混淆的情况. 
    #如果用文本保存起来, 那么以后可以反混淆, 还原文件
    print "Start Confuse ...." 
    for j in range(0, len(raw_name_list)) :
        print raw_name_list[j] , " --> " , new_name_list[j] 
    print "Confuse Complete !" 
