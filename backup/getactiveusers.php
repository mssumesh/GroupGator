<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="refresh" content="1500">
<title>Active Users</title>
</head>

<body>
<?php
$con = mysql_connect("localhost", "groupgat_gator", "asdf123456");
mysql_select_db("groupgat_paypal");

$result = mysql_query("SELECT * FROM paypal_users WHERE timestamp > " . (time() - 130));

echo "<h3>Active users: " . mysql_num_rows($result) . "</h3>";
while($row = mysql_fetch_array($result)){
	echo $row["email"] . " connected " . (time() - $row["timestamp"]) . " seconds ago<br />";
}

$result = mysql_query("SELECT * FROM free_users WHERE timestamp > " . (time() - 130));

echo "<h3>Active free users: " . mysql_num_rows($result) . "</h3>";
while($row = mysql_fetch_array($result)){
	echo $row["ip"] . " connected " . (time() - $row["timestamp"]) . " seconds ago<br />";
}
mysql_close($con);
?>
</body>
</html>