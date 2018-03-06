#! /usr/bin/env python
# coding=utf-8
# version: 2018-03-06 16:53:23

import os
import shutil


def rm_dir(source_dir, target_dir):
    list = os.listdir(target_dir)
    for line in list:
        new_path = os.path.join(source_dir, line)
        if os.path.exists(new_path):
            file_path = os.path.join(target_dir, line)
            if os.path.isdir(file_path) and os.path.isdir(new_path):
                shutil.rmtree(new_path)
            elif os.path.isfile(file_path) and os.path.isfile(new_path):
                os.remove(new_path)

def copy_dir_files(source_dir, target_dir):
    if os.path.isdir(source_dir) is False:
        shutil.copy(source_dir, target_dir)
        return
    for f in os.listdir(source_dir):
        sf = os.path.join(source_dir, f)
        tf = os.path.join(target_dir, f)
        if os.path.isfile(sf):
            if not os.path.exists(target_dir):
                os.makedirs(target_dir)
            shutil.copy(sf, tf)
        if os.path.isdir(sf):
            copy_dir_files(sf, tf)

if __name__ == '__main__':
    # TODO:Unity工程路径，根据实际情况配置
    project_path = 'E:/WorkSpace/Trunk/client/PetsPlanet/'
    
    # 下面是通用的，不用修改
    sdk_path = './assets/'
    project_path = os.path.join(project_path, './ExportAndroid/pets/assets/')
    rm_dir(sdk_path, project_path)
    copy_dir_files(project_path, sdk_path)
