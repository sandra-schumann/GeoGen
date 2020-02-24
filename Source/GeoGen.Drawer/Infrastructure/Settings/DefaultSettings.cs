﻿using GeoGen.Infrastructure;
using System.Collections.Generic;

namespace GeoGen.Drawer
{
    /// <summary>
    /// The default <see cref="Settings"/>
    /// </summary>
    public class DefaultSettings : Settings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSettings"/> class
        /// </summary>
        public DefaultSettings() : base
        (
            loggingSettings: new LoggingSettings
            (
                loggers: new List<BaseLoggerSettings>
                {
                    // Console logger
                    new ConsoleLoggerSettings
                    (
                        includeLoggingOrigin: false,
                        includeTime: false,
                        logOutputLevel: LogOutputLevel.Info
                    ),

                    // File logger
                    new FileLoggerSettings
                    (
                        includeLoggingOrigin: true,
                        includeTime: true,
                        logOutputLevel: LogOutputLevel.Debug,
                        fileLogPath: "log.txt"
                    )
                }
            ),
            metapostDrawerSettings: new MetapostDrawerSettings
            (
                drawingData: new MetapostDrawingData
                (
                    scaleVariable: "u",
                    shiftLength: 0.05,
                    boundingBoxOffset: 0.01,
                    pointLabelMacro: "LabelPoint",
                    pointMarkMacros: new Dictionary<ObjectDrawingStyle, string>
                    {
                        { ObjectDrawingStyle.AuxiliaryObject, "PointMarkAuxiliaryStyle" },
                        { ObjectDrawingStyle.NormalObject, "PointMarkNormalStyle" },
                        { ObjectDrawingStyle.TheoremObject, "PointMarkTheoremStyle" }
                    },
                    lineSegmentMacros: new Dictionary<ObjectDrawingStyle, string>
                    {
                        { ObjectDrawingStyle.AuxiliaryObject, "LineSegmentAuxiliaryStyle" },
                        { ObjectDrawingStyle.NormalObject, "LineSegmentNormalStyle" },
                        { ObjectDrawingStyle.TheoremObject, "LineSegmentTheoremStyle" }
                    },
                    circleMacros: new Dictionary<ObjectDrawingStyle, string>
                    {
                        { ObjectDrawingStyle.AuxiliaryObject, "CircleAuxiliaryStyle" },
                        { ObjectDrawingStyle.NormalObject, "CircleNormalStyle" },
                        { ObjectDrawingStyle.TheoremObject, "CircleTheoremStyle" }
                    },
                    textMacro: "TexTextOnTheRight"
                ),
                metapostCodeFilePath: "figures.mp",
                metapostMacroLibraryPath: "macros.mp",
                constructionTextMacroPrefix: "_Text",
                compilationCommand: (program: "mpost", arguments: "-interaction=nonstopmode -s prologues=3"),
                postcompilationCommand: "post-compile.bat",
                logCommandOutput: true,
                numberOfPictures: 20
            ),
            drawingRuleProviderSettings: new DrawingRuleProviderSettings(filePath: "..\\..\\..\\drawing_rules.txt"),
            reorderObjects: true
        )
        { }
    }
}