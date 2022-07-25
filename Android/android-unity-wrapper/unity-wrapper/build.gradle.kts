plugins {
    id("com.android.library")
    id("kotlin-android")
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
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_1_8
        targetCompatibility = JavaVersion.VERSION_1_8
      }
}

dependencies {
    implementation("org.jetbrains.kotlin:kotlin-stdlib:${rootProject.extra["kotlin_version"]}")
    compileOnly(fileTree(mapOf("dir" to "libs", "include" to listOf("*.jar"))))
    compileOnly(Deps.appCompat)
    compileOnly(Deps.moengage)
    api(Deps.basePlugin)
    compileOnly(project(":unity-library"))
}

apply(plugin="com.vanniktech.maven.publish")