<?php
require 'ConnectSetting.php';

// variables submited by user
$loginUser = $_POST["User"];
$loginPass = $_POST["Pass"];

$stmt = $conn -> prepare("SELECT id,password FROM user Where username = (?)");
$stmt-> bind_param('s', $loginUser);
$stmt-> execute();
$result = $stmt-> get_result();

if($stmt -> errno == 0){
    if($result ->num_rows > 0){
        while($row = $result->fetch_assoc()){
            $isPasswordCorrect = password_verify($loginPass, $row["password"]);
            if($isPasswordCorrect == true) {
                echo $row["id"]; // input of password correct
            }else {
                echo -2; // input of password error.
            }
        }
    }else{
        echo -3; // no username matches.
    }
}else{
    echo -4; // database error.
}



$conn -> close();
?>