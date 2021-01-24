<?php
session_start();

$loginErrors = Array();
$registerErrors = Array();
$captchaErrors = Array();

$con = mysql_connect("localhost", "groupgat_gator", "asdf123456");
mysql_select_db("groupgat_paypal", $con);


if(isset($_SESSION["email"]) == true){
	header("location:index.php");
}
else if(isset($_REQUEST["email"]) == true && isset($_REQUEST["password"]) == true){
	$email 		= mysql_real_escape_string(strtolower($_REQUEST["email"]));
	$password 	= md5(mysql_real_escape_string($_REQUEST["password"]));
	$result 	= mysql_query("SELECT * FROM paypal_users WHERE email='$email' AND password='$password'", $con);
	
	if(mysql_num_rows($result) == 1){
		$_SESSION["email"] = $email;
		header("location:index.php");
	}
	else{
		array_push($loginErrors, "<p>Attempted username and password combination does not exist</p>");
	}
}
else if(isset($_POST["register_email"]) == true && isset($_POST["register_password1"]) == true && isset($_POST["register_password2"]) == true){
	if(isset($_POST["recaptcha_challenge_field"]) == true){
		require_once('recaptchalib.php');
		$privatekey = "6LdBzskSAAAAAAlhmhKaBIzk9o9vBy2zGEjC5KSz";
		$resp = recaptcha_check_answer ($privatekey,
									$_SERVER["REMOTE_ADDR"],
									$_POST["recaptcha_challenge_field"],
									$_POST["recaptcha_response_field"]);
		if (!$resp->is_valid) {
			$error = $resp->error;
			
			if(strcmp($error, "incorrect-captcha-sol") == 0)
				array_push($captchaErrors, "<p>The captcha you entered was incorrect</p>");
			else
				array_push($captchaErrors, "<p>" . $error . "</p>");
		} else {
			$email 		= mysql_real_escape_string(trim(strtolower($_POST["register_email"])));
			$password1 	= md5(mysql_real_escape_string($_POST["register_password1"]));
			$password2 	= md5(mysql_real_escape_string($_POST["register_password2"]));
			
			if(checkEmail($email) == true)
			{
				if(strlen($_POST["register_password1"]) >= 6)
				{
					if(strcmp($password1, $password2) == 0){
						if(mysql_num_rows(mysql_query("SELECT * FROM paypal_users WHERE email='$email'", $con)) == 0){
							mysql_query("INSERT INTO paypal_users VALUES(null, '$email', '$password1', 0, '', 0)", $con);
							$_SESSION["email"] = $email;
							header("location:index.php");
						}
						else
							array_push($registerErrors, "<p>This email is already registered</p>");
					}
					else
						array_push($registerErrors, "<p>Passwords do not match</p>");
				}
				else
					array_push($registerErrors, "<p>Password must be at least 6 characters long</p>");
			}
			else
				array_push($registerErrors, "<p>Invalid email format</p>");
		}
	}
}

$title = 'Login or Donate to unlock Unlimited Version!'; include 'header.php';

function checkEmail($email){
  if (preg_match('/^[^0-9][a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[@][a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[.][a-zA-Z]{2,4}$/',$email))
		return true;
	else 
		return false;
}
?>

<!--[if IE]>
<div id="iesucks" style="position: fixed; top: 15px; left: 20px; font-weight: bold; color: yellow; width: 300px; height: auto; font-size: 10px;"><h1>We noticed you're using Internet Explorer to view our site. We do not support IE. We highly recommend you download Firefox, Google Chrome or Safari instead of using IE. </h1></div>
<![endif]-->

<div id="google_translate_element" style="position: fixed; top: 0; right: 0;"></div>
<script>
function googleTranslateElementInit() {
  new google.translate.TranslateElement({
    pageLanguage: 'en',
    gaTrack: true,
    gaId: 'UA-26934555-1',
    layout: google.translate.TranslateElement.InlineLayout.SIMPLE
  }, 'google_translate_element');
}
</script>
<script src="//translate.google.com/translate_a/element.js?cb=googleTranslateElementInit"></script>

<div id="rightCol">
	<h2>Group GATOR is an invite tool developed for Steam groups, more specifically, steam group owners.  It gathers and invites users to your Steam group with a few clicks. Within seconds you're inviting new users to your group. Group GATOR has 24/7 365 support through our <a id="forum" href="http://groupgator.enjin.com/forum/" title="Register and view or post in the forum." target="_new">forums</a>. Group GATOR updates and upgrades are always free to our users.</h2>
<div class="step_contents">
			<h3>Download the latest version</h3>
			<div class="download">
			<p>Coming Soon!</p>
				<!--<p><a href="http://files.enjin.com/111304/GATOR/GroupGATOR1007ZIP.zip" id="download_button" onclick="_gaq.push(['_trackEvent','DownloadORG',this.href]);">Download</a></p>
				<p class="version">Current Version <span class="version_number">1.0.0.7</span></p>-->
			</div>
	</div>
	<div class="step_contents">
		<h3>Login</h3>
		<div class="login">
			<form action="" method="POST">
				<table>
					<tr>
						<td><label for="email">Email</label></td><td><input type="text" name="email" /></td>
					</tr>
					<tr>
						<td><label for="password">Password</label></td><td><input type="password" name="password" /></td>
					</tr>
				</table>
				<input class="button" type="submit" value="Login" />
				<div class="error_message">
					<?php echo implode("", $loginErrors); ?>
				</div>
			</form>
		</div>
	
	
		<h3>Register</h3>
		<div class="register">
			<form action="" method="POST">
				<table>
					<tr>
						<td><label for="register_email">PayPal Email</label></td><td><input  name="register_email" type="text" value="PayPal Email (HIGHLY RECOMMENDED!)" /></td>
					</tr>
					<tr>
						<td><label for="register_password1">Password</label></td><td><input name="register_password1" type="password" /></td>
					</tr>
					<tr>
						<td><label for="register_password2">Confirm Password</label></td><td><input type="password" name="register_password2" /></td>
					</tr>
				</table>
				<div class="recaptcha">
					<?php
					  require_once('recaptchalib.php');
					  $publickey = "6LdBzskSAAAAADDuX5Abq6eyRD2jVGtqDgWD9Wfi";
					  echo recaptcha_get_html($publickey);
					?>
				</div>
				<input class="button" type="submit" value="Register" />
				<div class="error_message">
					<?php echo implode("", $registerErrors); echo implode("", $captchaErrors); ?>
				</div>
			</form>
		</div>
		
	</div>
</div>

<div id="leftCol">
	<div id="leftCol2">
		<img src="images/01autoharvest.png" alt="Auto Gather" />
		<h2>Pick a group and Group GATOR gathers the users. It creates a gather list which Group GATOR uses to invite users to your group. You can save these lists and load previously saved lists any time.</h2>
		
		<img src="images/02autoInvite.png" alt="Auto Invite" />
		<h2>With one click you can begin to invite the users. <strong>Watch in real time as your group count increases!</strong></h2>
		
		<img src="images/03target.png" alt="Target Users" />
		<h2>Select which users to gather based on online status. Target more active users for your group! More active users is not only good for your group, but also for your game servers.</h2>
		
		<img src="images/04blacklisting.png" alt="blacklisting" />
		<h2>Blacklisting prevents harvesting users you've already invited. There are several blacklist options to play with. Very useful! Also prevents spam.</h2>
		
		<img src="images/05speedSettings.png" alt="Speed Settings" />
		<h2>Choose between 3 invite speed settings for the absolute best results.</h2>
		
		<img src="images/09freeUnlimited.png" alt="Free and Unlimited Version of Group GATOR" />
		<h2>Free version gives you a taste of what the Group GATOR can do. Unlimited gives you all the options.</h2>
		
		<img src="images/06unlimitedInvites.png" alt="Unlimited Invites (Donators Only)" />
		<h2>Unlimited version is reserved for users who support the project by donating. If you've used the free version successfully you know what Group GATOR is capable of. Get all the options and unlimited invites. Donate today!</h2>
		
		<img src="images/07secure.png" alt="Secure" />
		<h2>Extremely secure to protect your account and account information.</h2>
		
		<img src="images/08safe.png" alt="Safe" />
		<h2>Group GATOR does not spam or perform any illegal functions. Group GATOR does not gather or store any of your Steam account information. Group GATOR is not a hack and will not get you VAC banned.</h2>
		
	</div>
</div>
<?php include 'footer.php'; ?>
