# Readme in development
Release soon!

# Snowrunner-Patcher

Snowrunner-Patcher is an app that automate an ModPak for the game.<br>
Snowrunner uses inital.pak file to load the modifications of the game.

# What is the porpouse?

This app can patch the current modpak installed and replace any files in the modpak that are modded.
This can be very usefull if you play with friends and you are changing things or adding mods to the game so all of you can have the same modpaks.

# SETUP

1. Download the last update avaiable on the releases page, and install it. 
2. The first time you open it a dialog will show that indicates where the current modpak is installed, you will check where the game is installed and go to -> /en_us/preload/paks/client/**inital.pak**.
3. Open config file (Menu-> Open Config File), then in the config file you will need to put the link of your modPak.
4. Optionally you can put the link of the mod version (this can only needs to contains a version like 1.0.X.X).
5. Optionally token if your modpak is hosted on github.
6. Restart the application.

> [!IMPORTANT]
> The link provided fro the modPak download needs to be a raw content link, for example  https://raw.githubusercontent.com/.. <br>
> Snowrunner-Patcher can check version of the modPak if you put yor version link in the config. If any link is provided the normal/forced instalation always can be executed.

> [!NOTE]
> If your game does not start you can restore last backup from menu ModPak->Backups->Replace Modpak->Last Backup

## Config
Snowrunner Patcher uses an config.ini to set up all the things to patch the game correctly

### Configurable Options
Most of this configurations can be configured  in the app.

* **PatchingMode**: is marked with Advanced by default.
* **ModsPath**: Current modpath folder installation. (can be configured in app)
* **UrlDownload**: Where is the endpoint to download the modPak File.
* **UrlVersion**: Where Snowrunner-Patcher will check version. **More info below**.
* **Token**: This is intended for the Github token but it can be used if your platform where you host your modpak. This option allow to add a header in the request -> `Authorization` and the value as `token {YourKey}` where your key is the text putted on this config.

If you edit the config file you will need to restart the application.




