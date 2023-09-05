dotnet restore source/mapbox-android/mapbox-android.sln

# only build the plugin projects. these will build all the other projects as dependencies
# only clean the first built process. not cleaning the subsequent ones will help to not have to rebuild dependencies
dotnet build -t:Clean,Build -p:Configuration=Release ./source/mapbox-android/com.mapbox.mapboxsdk.mapbox-android-plugin-annotation/com.mapbox.mapboxsdk.mapbox-android-plugin-annotation.csproj
dotnet build -t:Build -p:Configuration=Release ./source/mapbox-android/com.mapbox.mapboxsdk.mapbox-android-plugin-building/com.mapbox.mapboxsdk.mapbox-android-plugin-building.csproj
dotnet build -t:Build -p:Configuration=Release ./source/mapbox-android/com.mapbox.mapboxsdk.mapbox-android-plugin-traffic/com.mapbox.mapboxsdk.mapbox-android-plugin-traffic.csproj

# move files to a common directory
# note: after moving plugin-annotation, all dependencies are built and copied. only need to move the 2 other specific plugin files

rm -rf ./output
mv -f ./source/mapbox-android/com.mapbox.mapboxsdk.mapbox-android-plugin-annotation/bin/Release/net7.0-android/ ./output
mv -f ./source/mapbox-android/com.mapbox.mapboxsdk.mapbox-android-plugin-building/bin/Release/net7.0-android/com.mapbox.mapboxsdk.mapbox_android_plugin_building.* ./output
mv -f ./source/mapbox-android/com.mapbox.mapboxsdk.mapbox-android-plugin-traffic/bin/Release/net7.0-android/com.mapbox.mapboxsdk.mapbox_android_plugin_traffic.* ./output
