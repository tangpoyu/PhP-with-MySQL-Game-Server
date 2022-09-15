<?php
require 'ConnectSetting.php';

$userId = $_POST["userId"];

$sql = $conn -> prepare("SELECT itemId FROM useritem WHERE userId = ?");
$sql -> bind_param("i",$userId );
$sql -> execute();
$result = $sql ->get_result();

if($result -> num_rows > 0) {
    $rows = array();
    while($row = $result->fetch_assoc()){
        $rows[] = $row;
    }
    echo json_encode($rows);
}else {
    echo 0;
}

$conn -> close();
?>