// Top-level build file where you can add configuration options common to all sub-projects/modules.
buildscript {
  val kotlin_version by extra("1.3.72")
    repositories {
    mavenLocal()
    mavenCentral()
    google()
    mavenCentral()
    jcenter()
  }
  dependencies {
    classpath("com.android.tools.build:gradle:4.1.2")
    classpath("com.vanniktech:gradle-maven-publish-plugin:0.13.0")
    classpath("org.jetbrains.kotlin:kotlin-gradle-plugin:1.4.20")
    classpath("org.jetbrains.dokka:dokka-gradle-plugin:1.4.32")
  }
}

allprojects {
  repositories {
    google()
    mavenCentral()
    jcenter()
  }
}

tasks.register<Delete>("clean") {
  delete(rootProject.buildDir)
}