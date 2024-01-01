# Readme in development
Release soon!

# Snowrunner-Patcher

Snowrunner-Patcher is an app that automates a ModPak for the game.  
Snowrunner uses the `initial.pak` file to load the modifications of the game.

# What is the porpouse?

This app can patch the current ModPak installed and replace any files in the ModPak that are modded.  
This can be very useful if you play with friends and you are changing things or adding mods to the game so all of you can have the same ModPaks.

# SETUP

1. Download the latest update available on the releases page, and install it.
2. The first time you open it, a dialog will show that indicates where the current ModPak is installed. You will check where the game is installed and go to `/en_us/preload/paks/client/**initial.pak**`.
3. Open the config file (Menu -> Open Config File), then in the config file, you will need to put the link to your ModPak.
4. Optionally, you can put the link of the mod version (this only needs to contain a version like 1.0.X.X).
5. Optionally, a token if your ModPak is hosted on GitHub.
6. Restart the application.

> [!IMPORTANT]
> The link provided for the ModPak download needs to be a raw content link, for example, https://raw.githubusercontent.com/...<br>
> Snowrunner-Patcher can check the version of the ModPak if you put your version link in the config. If any link is provided, the normal/forced installation can always be executed.

> [!NOTE]
> If your game does not start, you can restore the last backup from the menu ModPak -> Backups -> Replace Modpak -> Last Backup

## Config (WIP)
Snowrunner Patcher uses a `config.ini` to set up all the things to patch the game correctly.

### Configurable Options
Most of this configurations can be configured  in the app.

* **PatchingMode**: It is marked with Advanced by default.
* **ModsPath**: Current ModPath folder installation. (can be configured in the app)
* **UrlDownload**: Where is the endpoint to download the ModPak File.
* **UrlVersion**: Where Snowrunner-Patcher will check the version. **More info below**.
* **Token**: This is intended for the GitHub token, but it can be used if your platform where you host your ModPak. This option allows adding a header in the request -> `Authorization` and the value as `token {YourKey}` where your key is the text putted on this config.

If you edit the config file, you will need to restart the application.




