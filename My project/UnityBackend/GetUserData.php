<?php
require 'ConnectSetting.php';

$userId = $_POST["userId"];

$sql = $conn -> prepare("SELECT username, level, coin FROM user WHERE id = ?");
$sql -> bind_param("i", $userId);
$sql -> execute();
$result = $sql -> get_result();

if($result -> num_rows > 0) {
    while($row = $result->fetch_assoc()){
        echo json_encode($row);
    }
}else {
    echo 0;
}

$conn -> close();
?>