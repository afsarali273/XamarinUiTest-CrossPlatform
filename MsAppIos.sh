#!/bin/bash
# This is a Xamarin Testcloud test upload shell script

### This will have to be updated when Xamarin.UITest is updated via NuGet.

### You will have to update these variables for your environment
export CURRENTDATE=`date +"%Y-%m-%d %I.%M.%S %p"`
export PRJLOC="full path of project"
export IOS_DEVICE_ID="hlogzaogza/ios"
export TEST_ASSEMBLIES="${PRJLOC}\MobileApps\MobileApps\bin\Debug"
export APK="*fullpath*\MyVodacom_Hockey_9.9.ipa"
export SERIES="master"
export APPNAME="hlogzaogza/VSP_IOS"
export LOCALE=en_US
export TESTDATA1="${PRJLOC}\MobileApps\MobileApps\bin\Debug\Envtestdata"
export TESTDATA2="${PRJLOC}\MobileApps\MobileApps\bin\Debug\EnvtestOR"
export XMLRESULTLOC="${APPNAME}\XTCresults[${CURRENTDATE}].xml"
export Logloc="*Full path*\Log\[${CURRENTDATE}].log"

appcenter test run uitest --app "${APPNAME}" --devices "${IOS_DEVICE_ID}" --app-path "${APK}" --test-series "${SERIES}" --locale "${LOCALE}" --build-dir "${TEST_ASSEMBLIES}" --include "${TESTDATA1}" --include "${TESTDATA2}" 



