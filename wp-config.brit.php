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
define('DB_NAME', 'britwordpress');

/** MySQL database username */
define('DB_USER', 'brit');

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
 define('AUTH_KEY',         '`c(?YN&p_2+KQ):cX4H:oWN3~|~bRv;vxL0]ES/P$-fu)5cOZ Q|Z$Y[Bh5q:l_S');
 define('SECURE_AUTH_KEY',  '>HcJ{e|)U~47M.S5&4k#GH<,+aQa.(*V3@pdc:2pc&eK]Dc%~>Z9=vTU;sUY>&k=');
 define('LOGGED_IN_KEY',    'zQ-=ieW3/_5;!|:a _[Tk*?6@N{lU@ACV<Wd@*NVC>k*I6YUq)p;R9L/y+%Us)N,');
 define('NONCE_KEY',        'sV4tCs+SU4M%p.pfsxfM488|<t&-1D]nAV]7h-.WCRK)2 ;jbk^mQp9GOp%4RE;9');
 define('AUTH_SALT',        'lCXDF@H?7<[Rx}B]h]E?9U{MPR|]D-,|`M!hKT%7/CrjI%>M=<BH+v6]=rzZ^BKD');
 define('SECURE_AUTH_SALT', '+zZnj}yc+.f{Rp`O]fn=EmoQ+!Ru]v_ZAE-&rQl wt*8;PO]q=omg(fo9q?;@D%H');
 define('LOGGED_IN_SALT',   'HD9wcL-ZhS.{mo|wg]Pmn/rQ FD&ds{-5CsOAg$u-gh4`Dc_#a3~nbvxO0%c7V m');
 define('NONCE_SALT',       '~`q%>MH0BiPa:g|p/>=zoIq*oqRcqB>+6]mDXlT~oTdBYLvUB6DB_r.9K|t/GNjh');

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
