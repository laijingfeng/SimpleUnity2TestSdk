#! /usr/bin/env python
#coding=utf-8
#version: 2017-05-26-00

import os, shutil

def rm_dir(path):
    if os.path.isdir(path):
        shutil.rmtree(path)
        
# 拷贝目录下的文件
def CopyDirFiles(sDir, tDir):
    if os.path.isdir(sDir) == False:
        return
    
    for f in os.listdir(sDir):
        sf = os.path.join(sDir, f)
        tf = os.path.join(tDir, f)
        if os.path.isfile(sf):
            if not os.path.exists(tDir):
                os.makedirs(tDir)
            shutil.copy(sf, tf)
        if os.path.isdir(sf):
            CopyDirFiles(sf, tf)

if __name__ == '__main__':

    exoprt_and = './../UnityProject/ExportAndroid/UnityProject/assets/Android'
    exoprt_bin = './../UnityProject/ExportAndroid/UnityProject/assets/bin'
    
    sdk_and = './assets/Android'
    sdk_bin = './assets/bin'

    rm_dir(sdk_and)
    rm_dir(sdk_bin)
    
    CopyDirFiles(exoprt_and, sdk_and)
    CopyDirFiles(exoprt_bin, sdk_bin)
