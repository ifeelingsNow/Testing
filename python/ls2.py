a,b,c,d = 1, 1.2, 'abc', 5+4j
# a=1
# b=1.2
# c='abc'
# d=5+4j
# print后面只能接一种变量类型，连着写就不行
print('a=', end="")
print(a,end=" ")
print('b=',end="")
print(b,end=" ")
print('c=',end="")
print(c,end=" ")
print('d=',end="")
print(d,end=".\n")

print('a的类型为', end="")
print(type(a),end=" ")
print('b的类型为',end="")
print(type(b),end=" ")
print('c的类型为',end="")
print(type(c),end=" ")
print('d的类型为',end="")
print(type(d),end=".\n")

print('用isinstance()函数来判断是不是int, isinstance(a, int)的返回值是:', end=' ')
print(isinstance(a,int), end="\n")

str="Runoob"
# Runoob
# Runoo
# R
# noo
# noob
# RunoobRunoob
# RunoobTEST
print(str)
print(str[0:-1])
print(str[0])
print(str[2:-1])
print(str[2:])
print(str*2)
print(str+'TEST')

list = ['abcd', 786, 2.23, 'runoob', 70.2]
tinnylist = [123, 'runoob']
# ['abcd', 786, 2.23, 'runoob', 70.2]
# abcd
# [786, 2.23]
# [2.23, 'runoob', 70.2]
# [123, 'runoob', 123, 'runoob']
# ['abcd', 786, 2.23, 'runoob', 70.2, 123, 'runoob']
print(list)
print(list[0])
print(list[1:3])
print(2*list[2:4])

def reversestring(input):
    strlist=input.split(" ")
    # print(type(strlist))
    rstr=strlist[-1::-1]
    # print(rstr)
    output=" ".join(rstr)
    return output

rw = reversestring("i love you!")
print(rw)

tuple=('abcd', 786, 2.23, 'runoob', 70.2)
tinytuple=(123, 'runoob')
# ('abcd', 786, 2.23, 'runoob', 70.2)
# abcd
# (786, 2.23)
# (2.23, 'runoob', 70.2)
# (123, 'runoob', 123, 'runoob')
# ('abcd', 786, 2.23, 'runoob', 70.2, 123, 'runoob')
print(tuple)
print(tuple[0])
print(tuple[1:3])
print(tuple[2:5])
print(tinytuple*2) #输出2次
print(tuple+tinytuple) #连接元组输出
print(tuple[0:-1])
# 用大括号{}来创建集合，或者set()来创建空集合，不用大括号{}创建集合是因为大括号{}表示空字典
student={'Johncy', 'Cherry', 'Wendy', 'Sally', 'Emily'}
# a=set('1234567')
print(student)

# tinydic={'name':'Johncy', 'age':36, 'site':'28554811@qq.com'}
# print(type(tinydic))
# print(tinydic)

testtuple=('name','Johncy', 'age','36', 'site','28554811@qq.com')
print(type(testtuple))
print(testtuple)
testdic = dict(testtuple)
print(type(testdic))
print(testdic)