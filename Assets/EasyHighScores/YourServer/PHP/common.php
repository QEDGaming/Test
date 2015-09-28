<?php

$DatabaseName = 'fireg893_Test';
$secretKey = "123456";

function DatabaseConnect()
{
	global  $DatabaseName;
	global  $secretKey;

	$link = mysql_connect('localhost.firegod.net', 'fireg893_TW', 't0emBy$!o@)]');
	
	if(!$link)
	{
		fail("Unable to connect to your mySQL database");
	}
	if(!@mysql_select_db($DatabaseName))
	{
		fail("Can´t find your mySQL database $DatabaseName");
	}
	return $link;
	}
	
function safe($var)
{
	$var = addslashes(trim($var));
	return $var;
}

function fail($errorMsg)
{
	print $errorMsg;
	exit;
}

?>