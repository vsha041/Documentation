import re

with open('C:\\Users\\Vsha041\\Desktop\\filename.txt') as f:
    for line in f:
        match = re.search(r"'([^']*)'", line)
        if match:
            print('Update-Package ' + match.group(1) + ' -ProjectName Some.Name -reinstall')


# INPUT
#filename.txt contents
#<?xml version='1.0' encoding='utf-8'?>
#<packages>
#  <package id='Autofac' version='4.9.1' targetFramework='net461' />
#  <package id='Autofac.Mvc4' version='3.1.0' targetFramework='net452' />
#</packages>

# OUTPUT
# Update-Package Autofac -ProjectName Some.Name -reinstall
# Update-Package Autofac.Mvc4 -ProjectName Some.Name -reinstall
