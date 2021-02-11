plugins {
    id("com.android.library")
}

ext {
    set(PomKeys.artifactId, ReleaseConfig.artifactId)
    set(PomKeys.description, ReleaseConfig.description)
    set(PomKeys.name, ReleaseConfig.artifactName)
    set(PomKeys.versionName, ReleaseConfig.versionName)
}

android {
    compileSdkVersion(SdkBuildConfig.compileSdkVersion)

    defaultConfig {
        minSdkVersion(SdkBuildConfig.minimumSdkVersion)
        targetSdkVersion(SdkBuildConfig.targetSdkVersion)
    }

    buildTypes {
        getByName("release") {
            isMinifyEnabled = false
            proguardFiles(getDefaultProguardFile("proguard-android.txt"), "proguard-rules.pro")
        }
    }
}

dependencies {
    compileOnly(fileTree(mapOf("dir" to "libs", "include" to listOf("*.jar"))))
    compileOnly(Deps.appCompat)
    compileOnly(Deps.moengage)
    api(Deps.basePlugin)
}

apply(plugin="com.vanniktech.maven.publish")