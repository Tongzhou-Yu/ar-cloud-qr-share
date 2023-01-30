<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>无标题文档</title>
</head>
<?php
function Create($myFile)
{
	header('content-type:text/html; charset=utf-8');//防止生成的页面乱码
	
	$title ="src=".$myFile; //定义变量
	echo $title;
	$temp_file = "temp.html"; //临时文件，也可以是模板文件
	$dest_file = "dest_page.html"; //生成的目标页面
	
	$fp = fopen($temp_file, "r"); //只读打开模板
	$str = fread($fp, filesize($temp_file));//读取模板中内容
	
	$str = str_replace("[src=]", $title, $str);//替换内容
	fclose($fp);
	
	$handle = fopen($dest_file, "w"); //写入方式打开需要写入的文件
	fwrite($handle, $str); //把刚才替换的内容写进生成的HTML文件
	fclose($handle);//关闭打开的文件，释放文件指针和相关的缓冲
}
?>
<?php
 
$myFile = $_FILES["post"]["tmp_name"];
$content = '';
$fh = fopen($myFile, 'r') or die("can't open file");
while (!feof($fh)) {
    $content .= fgets($fh);//filesize($myFile)) or die('can\'t read');
}
fclose($fh);
 
 //文件存储路径
$file_path="upload/";
 if(is_dir($file_path)!=TRUE) mkdir($file_path,0664) ;
$myFile = $file_path.$_REQUEST['Name'].".png";
$fh = fopen($myFile, 'w') or die("can't open file");
//$stringData = $_FILES["fileUpload"];
$stringData = $content;//"START:\n" . join(',\n',headerCustom()) . ' \END';
fwrite($fh, $stringData);
fclose($fh);
echo $myFile;
Create($myFile);
?>
<body>
</body>
</html>