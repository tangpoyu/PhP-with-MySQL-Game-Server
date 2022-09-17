<?php
require 'ConnectSetting.php';

$username = $_POST["User"];
$password = $_POST["Pass"];


$stmt = $conn -> prepare("SELECT username FROM user Where username = (?)");
$stmt-> bind_param('s', $username);
$stmt-> execute();
$result = $stmt-> get_result();

if($result ->num_rows > 0){
    echo -2; // "This username is already taken."
}else {
    $stmt = $conn -> prepare("INSERT INTO user (username, password, level, coin) VALUES (?,?,1,0)");
    $stmt -> bind_param("ss", $username, $password);
    $stmt -> execute();
    $result = $stmt-> get_result();
    if($stmt -> errno == 0){
        $stmt = $conn -> prepare("SELECT id FROM user Where username = (?)");
        $stmt-> bind_param('s', $username);
        $stmt-> execute();
        $result = $stmt-> get_result();
        if($stmt -> errno == 0){
            if($result -> num_rows > 0){
                echo $result -> fetch_assoc()["id"]; // success build account.
            }else{
                echo -1; //error
            }
        }
        else{
            echo -1; //error
        }
    }else{
        echo -1; //error
    }
}

$conn -> close();
?>