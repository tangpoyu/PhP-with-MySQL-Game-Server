<?php
require 'ConnectSetting.php';

$ItemId = $_POST["itemId"];


$sql = $conn -> prepare("SELECT name, description, price, imageUrl FROM item WHERE ID = ?");
$sql -> bind_param("i", $ItemId);
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