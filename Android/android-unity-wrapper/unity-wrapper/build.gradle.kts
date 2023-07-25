plugins {
    id("com.android.library")
    id("kotlin-android")
}

android {
    compileSdk = SdkBuildConfig.compileSdkVersion

    defaultConfig {
        minSdk = SdkBuildConfig.minimumSdkVersion
        targetSdk = SdkBuildConfig.targetSdkVersion
    }

    buildTypes {
        getByName("release") {
            isMinifyEnabled = false
            proguardFiles(getDefaultProguardFile("proguard-android.txt"), "proguard-rules.pro")
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_1_8
        targetCompatibility = JavaVersion.VERSION_1_8
      }
}

dependencies {
    compileOnly(fileTree(mapOf("dir" to "libs", "include" to listOf("*.jar"))))
    compileOnly(Deps.appCompat)
    compileOnly(Deps.moengage)
    compileOnly(Deps.inapp)
    compileOnly(Deps.geofence)
    api(Deps.basePlugin)
    implementation("org.jetbrains.kotlin:kotlin-stdlib:1.6.0")
    compileOnly(project(":unity-library"))
}

apply(plugin = "com.vanniktech.maven.publish")