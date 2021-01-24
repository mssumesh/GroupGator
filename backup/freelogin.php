<?php

$con = mysql_connect("localhost", "groupgat_gator", "asdf123456");
mysql_select_db("groupgat_paypal", $con);

$ip = $_SERVER["REMOTE_ADDR"];
$m_id  = mysql_real_escape_string(strtolower($_REQUEST["uniqueid"]));
$curu = mysql_real_escape_string(strtolower($_REQUEST["current"]));
$status = mysql_real_escape_string(strtolower($_REQUEST["status"]));
$curusage = abs ($curu);



$result = mysql_query("SELECT * FROM free_users WHERE mcode='$m_id'", $con);
$remaining = 1000;
$exiting = 0;

if(mysql_num_rows($result) == 0  )
		{
		mysql_query("INSERT INTO free_users VALUES(null,'$ip','$m_id','$remaining'," . time() . ")");
		echo $remaining;
		}
else
		{
		if ( (mysql_num_rows($result) > 1  ) )
				{
				echo "-1";
				}
		else
				{
			
				$row = mysql_fetch_array($result);
				$remaining = $row["remuse"];
				if( $remaining <= 0)
						echo "0";//no subscription
				else
						{
						$remaining = $remaining - $curusage;
						
						if ( $status == 0 )
							mysql_query("UPDATE free_users SET remuse='$remaining',timestamp='$exiting' WHERE mcode='$m_id'", $con);
						else
							{
							if ( $status == 2 )
								{	
								mysql_query("UPDATE free_users SET remuse='$remaining' WHERE mcode='$m_id'", $con);	
								}	
							else
								{
								mysql_query("UPDATE free_users SET remuse='$remaining',timestamp=" . time() . " WHERE mcode='$m_id'", $con);
								}
							}				
						echo $remaining;
						}
				}
		}
?>