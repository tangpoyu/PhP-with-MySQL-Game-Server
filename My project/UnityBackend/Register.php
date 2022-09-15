<?php
require 'ConnectSetting.php';

$username = $_POST["User"];
$password = $_POST["Pass"];

$stmt = $conn -> prepare("SELECT username FROM user Where username = (?)");
$stmt-> bind_param('s', $username);
$stmt-> execute();
$result = $stmt-> get_result();

if($result ->num_rows > 0){
    echo "This username is already taken.";
}else{
    echo "Username doesn't exist.";
    $stmt = $conn -> prepare("INSERT INTO user (username, password, level, coin) VALUES (?,?,1,0)");
    $stmt -> bind_param("ss", $username, $password);
    $stmt -> execute();
    $result = $stmt-> get_result();
    if($stmt -> errno == 0){
        echo "New record created successfully.";
    }else{
        echo "Error: " . $stmt -> error;
    }
}

$conn -> close();
?>