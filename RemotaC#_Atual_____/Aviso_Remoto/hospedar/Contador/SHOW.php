<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>:::::: LISTA DE INFECTADOS ::::::</title>
</head>

<body>

<?php
$f = "CONTADOR.txt";
$file = fopen($f, "r") or exit("Unable to open file!");
while(!feof($file))
  {
  echo fgets($file). "<br>";
  }
fclose($file);
?>

</body>
</body>
</html>