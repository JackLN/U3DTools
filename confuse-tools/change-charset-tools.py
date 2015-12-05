#! /usr/bin/env python
#coding=utf-8

############################################
# change_charset_tools.py
# python-version 2.7
# need chardet-package
#
# Anthor             Jack
# create-date        15.12.02
# discribe           change file charset
############################################

import chardet
import os
import sys
import io

def get_charset(file_data):
    print "Old Type is : ",chardet.detect(file_data)['encoding']
    file_chardet = chardet.detect(file_data)['encoding']
    return file_chardet

#Change one file
def change_file_charset(path_filename,chardet):
    f = file(path_filename)
    print path_filename
    data = f.read()
    f.close()

    if data!="":
        old_charset = get_charset(data)
        if chardet!=old_charset:
            file_content=""
            #f = file(path_filename)
            with io.open(path_filename,"r",encoding=old_charset) as f:
                for line in f:
                    line = line.encode("ascii","ignore")
                    file_content += line
            f = file(path_filename, 'w')
            #temp = data.decode(old_charset)
            #data = temp.encode(chardet)
            file_content=file_content.encode(chardet)
            f.write(file_content)
            f.close()
    
    
#change all
def change_to_type(file_type):
    print "Target type is : " , file_type
    dir = os.getcwd() 
    for root, dirs, filename in os.walk(dir):  
        for file in filename:  
            path_filename = os.path.join(root, file)
            if path_filename.endswith('.cs'): 
                change_file_charset(path_filename,file_type)
                print "Change CharSet File: ", path_filename 

#main
if __name__=="__main__":
    
    length = len(sys.argv)
    if length == 1:
        change_to_type("UTF-8-SIG")
    elif length == 2:
        change_to_type(sys.argv[1])
    


