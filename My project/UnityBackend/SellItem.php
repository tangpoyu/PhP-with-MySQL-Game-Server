<?php
require 'ConnectSetting.php';

$itemId = $_POST["itemId"];
$userId = $_POST["userId"];
// $itemId = 2;
// $userId = 1;

$sql = $conn -> prepare("SELECT price FROM item WHERE id = ?");
$sql -> bind_param("i", $itemId);
$sql -> execute();
$result = $sql -> get_result();

if($result -> num_rows > 0){
    $itemPrice = $result -> fetch_assoc() ["price"];
    $sql = $conn -> prepare("DELETE FROM useritem WHERE userId = ? AND itemId = ? LIMIT 1");
    $sql -> bind_param("ii", $userId, $itemId);
    $sql -> execute();
    if($sql -> errno == 0) {
        $sql = $conn -> prepare( "UPDATE user SET coin = coin + ? WHERE id = ?");
        $sql -> bind_param("ii", $itemPrice, $userId);
        $sql -> execute();
        if($sql -> errno == 0){
            echo 0;
        }
    }else{
        echo -1; // error happened in database.
    }
}

?>