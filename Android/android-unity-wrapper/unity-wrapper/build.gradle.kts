plugins {
    alias(moengageInternal.plugins.plugin.android.lib)
    alias(moengageInternal.plugins.plugin.kotlin.android)
}

android {
    namespace = "com.moengage.unity.wrapper"
    compileSdk = 33
    defaultConfig {
        minSdk = 21
        targetSdk = 33
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
    implementation(moengageInternal.kotlinStdLib)
    compileOnly(moengage.appCompat)
    compileOnly(moengage.core)
    compileOnly(moengage.inapp)
    api(moengage.basePlugin)
    compileOnly(project(":unity-library"))
}