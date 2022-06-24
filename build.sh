nuget restore source/mapbox-android

# only build the plugin projects. these will build all the other projects as dependencies
# only clean the first built process. not cleaning the subsequent ones will help to not have to rebuild dependencies
msbuild -t:Clean,Build -p:Configuration=Release ./source/mapbox-android/com.mapbox.mapboxsdk.mapbox-android-plugin-annotation/com.mapbox.mapboxsdk.mapbox-android-plugin-annotation.csproj
msbuild -t:Build -p:Configuration=Release ./source/mapbox-android/com.mapbox.mapboxsdk.mapbox-android-plugin-building/com.mapbox.mapboxsdk.mapbox-android-plugin-building.csproj
msbuild -t:Build -p:Configuration=Release ./source/mapbox-android/com.mapbox.mapboxsdk.mapbox-android-plugin-traffic/com.mapbox.mapboxsdk.mapbox-android-plugin-traffic.csproj

# move files to a common directory
# note: after moving plugin-annotation, all dependencies are built and copied. only need to move the 2 other specific plugin files

rm -r ./output
mv -f ./source/mapbox-android/com.mapbox.mapboxsdk.mapbox-android-plugin-annotation/bin/Release/ ./output
mv -f ./source/mapbox-android/com.mapbox.mapboxsdk.mapbox-android-plugin-building/bin/Release/com.mapbox.mapboxsdk.mapbox_android_plugin_building.* ./output
mv -f ./source/mapbox-android/com.mapbox.mapboxsdk.mapbox-android-plugin-traffic/bin/Release/com.mapbox.mapboxsdk.mapbox_android_plugin_traffic.* ./output
