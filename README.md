# 短网址生成器

## 演示地址
http://t.haojima.net/

## 原理
- 转短码：
- 1、根据自增主键id前面补0，如：00000123
- 2、倒转32100000
- 3、把倒转后的十进制转六十二进制（乱序后）
- 解析短码：
- 1、六十二进制转十进制，得到如：32100000
- 2、倒转00000123，得到123
- 3、根据123作为主键去数据库查询映射对象

## 注意
由于NET Core(2.1)还是没有System.Drawing程序集，图片二维码等操作只有通过第三方编写的组件如ZKWeb.System.Drawing，但是在Linux环境需要依赖libgdiplus组件。
- contos环境处理：yum install -y libgdiplus && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
- docker环境处理可参考：https://www.cnblogs.com/stulzq/p/9339250.html

## 效果图
![default](https://user-images.githubusercontent.com/5820324/44307118-3ce50580-a3cf-11e8-98b3-f60c2cb9c819.png)

## 数据表结构
```
SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for ShortURLs
-- ----------------------------
DROP TABLE IF EXISTS `ShortURLs`;
CREATE TABLE `ShortURLs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ShortURL` varchar(255) NOT NULL,
  `Url` varchar(255) NOT NULL,
  `CreationTime` datetime NOT NULL,
  `LastModificationTime` datetime NOT NULL,
  `AccessNumber` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=230 DEFAULT CHARSET=utf8;
SET FOREIGN_KEY_CHECKS=1;
```
