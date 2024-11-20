```
  _____                 _ 
 |  _____ _ _ __  _   _(_)
 | |_ / _` | '_ \| | | | |      > 翻译 <
 |  _| (_| | | | | |_| | |      一个使用.NET8开发的控制台翻译工具
 |_|  \__,_|_| |_|\__, |_|
                  |___/   
```

###

<div>
  <img src="https://img.shields.io/github/stars/zkhssb/Fanyi?&style=for-the-badge&logoColor=white%20style=%22margin-bottom:%205px;">
  <img src="https://img.shields.io/badge/.NET-8.0-%2324292e.svg?&style=for-the-badge&logoColor=white%20style=%22margin-bottom:%205px;" />
</a> 

本项目灵感来自于: [Fānyì](https://github.com/afc163/fanyi)

# 翻译器

- 百度翻译
- ChatGPT翻译

# 使用方法

> 使用前请先 [配置翻译器](Options.md)

![image](https://github.com/user-attachments/assets/2d5e399d-cdbb-46d5-88f6-366f1471e4c5)

```
./Fanyi.exe -c "Hello" -t "jp"
```

```
./Fanyi.exe -c "Hello" -t "cn"
```

## 全部参数

 -  `-c` `-content` 要翻译的文本
 -  `-t` `-to` 目标语言
 -  `-f` `-from` 源语言

`源语言`和`目标语言`需要填语言代码,如不指定本参数默认为`auto`。       
可以参考 [语言代码](Languages.md)

> Auto模式会在源语言和目标语言之间进行自动识别      
> 如果源语言是中文,则目标语言为英文,反之亦然。

# 控制台模式

> 不输入任何参数,直接启动程序 可以打开控制台模式

![image](https://github.com/user-attachments/assets/013a04d5-f068-4505-83d4-0b9325aeaa75)

## 快捷键

- `Shift` + `Enter`: 换行
- `Shift` + `←`: 选择源语言
- `Shift` + `→`: 选择目标语言
- `Shift` + `↑` 或 `Shift` + `↓` : 互换语言

![image](https://github.com/user-attachments/assets/3c26eef4-7d47-42b3-8652-be201cf7177e)
