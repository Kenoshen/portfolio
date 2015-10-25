<?php
/**
 * The base configuration for WordPress
 *
 * The wp-config.php creation script uses this file during the
 * installation. You don't have to use the web site, you can
 * copy this file to "wp-config.php" and fill in the values.
 *
 * This file contains the following configurations:
 *
 * * MySQL settings
 * * Secret keys
 * * Database table prefix
 * * ABSPATH
 *
 * @link https://codex.wordpress.org/Editing_wp-config.php
 *
 * @package WordPress
 */

// ** MySQL settings - You can get this info from your web host ** //
/** The name of the database for WordPress */
define('DB_NAME', 'mikewordpress');

/** MySQL database username */
define('DB_USER', 'mike');

/** MySQL database password */
define('DB_PASSWORD', 'Riley219');

/** MySQL hostname */
define('DB_HOST', 'localhost');

/** Database Charset to use in creating database tables. */
define('DB_CHARSET', 'utf8');

/** The Database Collate type. Don't change this if in doubt. */
define('DB_COLLATE', '');

/**#@+
 * Authentication Unique Keys and Salts.
 *
 * Change these to different unique phrases!
 * You can generate these using the {@link https://api.wordpress.org/secret-key/1.1/salt/ WordPress.org secret-key service}
 * You can change these at any point in time to invalidate all existing cookies. This will force all users to have to log in again.
 *
 * @since 2.6.0
 */
 define('AUTH_KEY',         'O?bAZ+hyC&sZEy:U+9xQ%R/c;-,+:n[pmm(zfk<]]O1 j[:3q{n#VmJ<}z)0]@lX');
 define('SECURE_AUTH_KEY',  '*t3s|xB<<#:C1WN@g,vFF*m9=X K%p<~8f^@/$F3bBMVV?@f[ART)`eV`(g1x&*h');
 define('LOGGED_IN_KEY',    'I334>j#EdgJ:>t_OK->(+(+?u.{&8jAQsp,|iMukm$:p2;cZRpenSf2-2AJ7|!YB');
 define('NONCE_KEY',        'RF&hXK#||-0C#U7J|H:|k{6<xSUd(]n96j l.[+@[q]T2x1?Ngeo.$-]2vsF G{v');
 define('AUTH_SALT',        'mP@-^EC|KE,!+bv 7@GGj-7e)K[[QR^XQMZ|vV6fjR|%vk#vtqOiwG{naxqF-jdc');
 define('SECURE_AUTH_SALT', 'eZb4-[jO4xgEf} 0j+EDoTLqII+,-r0]*[L4C*1eb(UKg^eX[D-OIWQp8-]uOIM_');
 define('LOGGED_IN_SALT',   ',s<a~gtoCLj-A. Dzpm2Lab<2JaZ_4tDGUyES_%.TjyWLVQ[_`v;AeH/}VZ%7ng,');
 define('NONCE_SALT',       'Fk+bT=5^MXxoD#WT5?X6u#iEx)ih,Js>&>+kiY_v G|Jt6E@L]*OnJ`LiGlm4_JM');

/**#@-*/

/**
 * WordPress Database Table prefix.
 *
 * You can have multiple installations in one database if you give each
 * a unique prefix. Only numbers, letters, and underscores please!
 */
$table_prefix  = 'wp_';

/**
 * For developers: WordPress debugging mode.
 *
 * Change this to true to enable the display of notices during development.
 * It is strongly recommended that plugin and theme developers use WP_DEBUG
 * in their development environments.
 *
 * For information on other constants that can be used for debugging,
 * visit the Codex.
 *
 * @link https://codex.wordpress.org/Debugging_in_WordPress
 */
define('WP_DEBUG', false);

/* That's all, stop editing! Happy blogging. */

/** Absolute path to the WordPress directory. */
if ( !defined('ABSPATH') )
        define('ABSPATH', dirname(__FILE__) . '/');

/** Sets up WordPress vars and included files. */
require_once(ABSPATH . 'wp-settings.php');
