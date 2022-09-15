<?php
require 'ConnectSetting.php';

// variables submited by user
$loginUser = $_POST["User"];
$loginPass = $_POST["Pass"];

$stmt = $conn -> prepare("SELECT id,password FROM user Where username = (?)");
$stmt-> bind_param('s', $loginUser);
$stmt-> execute();
$result = $stmt-> get_result();

if($result ->num_rows > 0){
    while($row = $result->fetch_assoc()){
        if($row["password"] == $loginPass) {
            echo $row["id"];
        }else {
            echo 0;
        }
    }
}else{
    echo 0;
}


$conn -> close();
?>