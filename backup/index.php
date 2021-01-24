<?php
$con = mysql_connect("localhost", "groupgat_gator", "asdf123456");
mysql_select_db("groupgat_paypal", $con);

if(isset($_POST["set_email"]) == true){
	$email= $_POST["set_email"];
	$days = $_POST["amount"];
	echo "Adding $days days<br />";
	$days *= 86400;
	$days += time();
	
	mysql_query("UPDATE paypal_users SET expire=$days WHERE email='$email'");
}
else if(isset($_POST["get_email"]) == true){
	$email	= $_POST["get_email"];
	
	$result = mysql_query("SELECT * FROM paypal_users WHERE email='$email'");
	$row = mysql_fetch_array($result);
	$daysLeft = $row["expire"];
	$daysLeft -= time();//ae_creator@hotmail.com
	$daysLeft /= 86400;
	
	if($daysLeft < 0)
		$daysLeft = 0;
}
?>

<h3>Get days left</h3>
<form action="" method="POST">
Email <input type="text" name="get_email" /><br />
<input type="submit" value="Get" />
</form>
<?php
	if(isset($daysLeft) == true)
		echo "Days left: $daysLeft<br />";
?>
<h3>Set days left</h3>
<form action="" method="POST">
Email: <input type="text" name="set_email" /><br />
Days left: <input type="text" name="amount" value="0"/><br />
<input type="submit" value="Set" />
</form>