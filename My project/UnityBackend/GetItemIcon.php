<?php
$itemId = $_POST["itemId"];

// $sql = $conn -> prepare("SELECT imageUrl FROM item WHERE ID = ?")

$path = "http://localhost/UnityBackend/ItemIcons/" . $itemId . ".png";
$image = file_get_contents($path);
echo $image;
?>