#! /usr/bin/env python
# -*- coding:utf-8 -*-  

import glob
import os
import re

# 状态  
S_INIT              = 0;  
S_SLASH             = 1;  
S_BLOCK_COMMENT     = 2;  
S_BLOCK_COMMENT_DOT = 3;  
S_LINE_COMMENT      = 4;  
S_STR               = 5;  
S_STR_ESCAPE        = 6;  

def delete_one_file(file_path):
    print file_path
    f = file(file_path)
    state = S_INIT;
    filecontext = ""
    while True:
        line = f.readline()
        if len(line) == 0:
            break

        for c in line:
            if state == S_INIT:
                if c == '/':
                    state = S_SLASH;
                elif c == '"':
                    state = S_STR;
                    filecontext += c;
                else:
                    filecontext += c;
            elif state == S_SLASH:
                if c == '*':
                    state = S_BLOCK_COMMENT;
                elif c == '/':
                    state = S_LINE_COMMENT;
                else:
                    filecontext += '/';
                    filecontext += c;
                    state = S_INIT;
            elif state == S_BLOCK_COMMENT:
                if c == '*':
                    state = S_BLOCK_COMMENT_DOT;
            elif state == S_BLOCK_COMMENT_DOT:
                if c == '/':
                    state = S_INIT;
                elif c == '*':
                    state = S_BLOCK_COMMENT_DOT;
                else:
                    state = S_BLOCK_COMMENT;
            elif state == S_LINE_COMMENT:
                if c == '\n' or c == '\n\r':
                    state = S_INIT;
                    filecontext += c;
            elif state == S_STR:
                if c == '\\':
                    state = S_STR_ESCAPE;
                elif c == '"':
                    state = S_INIT;
                filecontext += c;
            elif state == S_STR_ESCAPE:
                state = S_STR; 
                filecontext += c;
    f.close() 
    
    f = file(file_path,'w')
    f.write(filecontext)
    f.close()

def delete_all_file():
    dir = os.getcwd()
    for root, dirs, filename in os.walk(dir):  
        for file in filename:  
            path_filename = os.path.join(root, file)
            if path_filename.endswith('.cs'): 
                delete_one_file(path_filename)        

#main
if __name__=="__main__":
    delete_all_file()
    
