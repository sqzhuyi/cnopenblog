<?php
// The file
$filename = 'mn.jpg';

// Content type
header('Content-type: image/jpeg');

// Get new dimensions
list($width, $height) = getimagesize($filename);
$new_width = $_POST['x2']-$_POST['x1'];
$new_height = $_POST['y2']-$_POST['y1'];

// Resample
$image_p = imagecreatetruecolor($new_width, $new_height);
$image = imagecreatefromjpeg($filename);
imagecopyresampled($image_p, $image, 0, 0, $_POST['x1'], $_POST['y1'], $new_width, $new_height, $new_width, $new_height);

// Output
imagejpeg($image_p, null, 100);
?> 