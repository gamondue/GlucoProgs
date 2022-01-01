SET SourceFilename=gamon logo_White_decentrated
REM SET SourceFilename =%1%
SET IconsFolder=..\GlucoMan.Mobile\GlucoMan.Mobile.Android\Resources\
%IconsFolder%
copy ".\Logos\%SourceFilename%_72.png" "%IconsFolder%mipmap-hdpi\icon.png"
copy ".\Logos\%SourceFilename%_48.png" "%IconsFolder%mipmap-mdpi\icon.png"
copy ".\Logos\%SourceFilename%_96.png" "%IconsFolder%mipmap-xhdpi\icon.png"
copy ".\Logos\%SourceFilename%_145.png" "%IconsFolder%mipmap-xxhdpi\icon.png"
copy ".\Logos\%SourceFilename%_192.png" "%IconsFolder%mipmap-xxxhdpi\icon.png"

copy ".\Logos\%SourceFilename%_162_launcher.png" "%IconsFolder%mipmap-hdpi\launcher_foreground.png"
copy ".\Logos\%SourceFilename%_108_launcher.png" "%IconsFolder%mipmap-mdpi\launcher_foreground.png"
copy ".\Logos\%SourceFilename%_216_launcher.png" "%IconsFolder%mipmap-xhdpi\launcher_foreground.png"
copy ".\Logos\%SourceFilename%_324_launcher.png" "%IconsFolder%mipmap-xxhdpi\launcher_foreground.png"
copy ".\Logos\%SourceFilename%_432_launcher.png" "%IconsFolder%mipmap-xxxhdpi\launcher_foreground.png"
