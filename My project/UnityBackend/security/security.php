<?php
require 'ConnectSetting.php';
require 'functions.php'; 

// $date = new Datetime('+1 week');
// setcookie('session','authentication thing', $date->getTimestamp());

if(!isset($_GET['username'])){
    die();
}

$username = $_GET['username'];
$user = $conn -> prepare("SELECT * FROM user WHERE username = ?");
$user -> bind_param("s",$username);
$user -> execute();
$result = $user -> get_result();
$user = $result -> fetch_assoc();
?>

<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <title>Page Title</title>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <link rel='stylesheet' type='text/css' media='screen' href='main.css'>
    <script src='main.js'></script>
</head>
<body>
    <h2><?php echo e($user["username"]) ?></h2>
    <p><?php echo e($user["bio"]) ?></p>
</body>
</html>


    