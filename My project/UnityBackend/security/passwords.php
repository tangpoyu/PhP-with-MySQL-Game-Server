<?php
$password = '$2y$12$XKinwEHUx.WEEpki90b0M.IzwtIJUxPF1iLJIH6hP9EYORVT3otxq';
$submittdPassword = 'ilovecats33';

$result = password_verify($submittdPassword, $password);
echo var_dump($result);

// echo password_hash('ilovecats33',PASSWORD_DEFAULT, ['cost' => 12]);