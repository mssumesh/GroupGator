<?php
$con = mysql_connect("localhost", "groupgat_gator", "asdf123456");
mysql_select_db("groupgat_paypal", $con);

$email		  = mysql_real_escape_string(strtolower($_REQUEST["email"]));
$password 	  = md5(mysql_real_escape_string($_REQUEST["password"]));
$computer_id  = mysql_real_escape_string(strtolower($_REQUEST["computer"]));
$lockcomputer = mysql_real_escape_string(strtolower($_REQUEST["lockcomputer"]));

$result = mysql_query("SELECT * FROM paypal_users WHERE email='$email' AND password='$password'", $con);

if(mysql_num_rows($result) != 1)
	echo "1";//No account
else{
	$row = mysql_fetch_array($result);
	
	if($row["expire"] <= 0 || $row["expire"] - time() <= 0)
		echo "2";//no subscription
	else if(strlen($row["computer_id"]) == 0){
		if(strcmp($lockcomputer, "true") == 0){
			mysql_query("UPDATE paypal_users SET computer_id='$computer_id' WHERE email='$email' AND password='$password'", $con);
			echo "6";
		}
		else
			echo "3";//no computer locked in yet
	}
	else if(strcmp($row["computer_id"], $computer_id) != 0)
		echo "4";//differnt computer
	else{
		echo "5";//valid session
		mysql_query("UPDATE paypal_users SET timestamp=" . time() . " WHERE email='$email' AND password='$password'");
	}
}
mysql_close($con);
?>