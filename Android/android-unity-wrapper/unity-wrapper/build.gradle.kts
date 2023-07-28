plugins {
    alias(moengageInternal.plugins.plugin.android.lib)
    alias(moengageInternal.plugins.plugin.kotlin.android)
}

val libVersionName = project.findProperty("VERSION_NAME") as String

apply(from = file("../scripts/common.gradle"))
apply(from = file("../scripts/release.gradle"))

android {
    namespace = "com.moengage.unity.wrapper"
    defaultConfig {
        buildConfigField("String", "MOENGAGE_ANDROID_UNITY_WRAPPER", libVersionName)
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
    implementation(moengageInternal.kotlinStdLib)
    compileOnly(libs.appCompat)
    compileOnly(libs.core)
    compileOnly(libs.inapp)
    api(libs.basePlugin)
    compileOnly(project(":unity-library"))
}