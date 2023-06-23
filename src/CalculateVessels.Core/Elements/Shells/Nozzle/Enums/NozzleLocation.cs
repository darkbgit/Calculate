namespace CalculateVessels.Core.Elements.Shells.Nozzle.Enums;

/// <summary>
/// 
/// </summary>
public enum NozzleLocation
{
    /// <summary>
    /// 1(cil, kon, ell)-Ось штуцера совпадает с нормалью к поверхности в центре отверстия
    /// </summary>
    LocationAccordingToParagraph_5_2_2_1,

    /// <summary>
    /// 2(cil, kon) - наклонный штуцер ось которого лежит в плоскости поперечного сечения
    /// </summary>
    LocationAccordingToParagraph_5_2_2_2,

    /// <summary>
    /// 3(ell) - смещенный штуцер ось которого паралелльна оси днища
    /// </summary>
    LocationAccordingToParagraph_5_2_2_3,

    /// <summary>
    /// 4(cil, kon) - наклонный штуцер максимальная ось симметрии отверстия некруглой формы составляет угол с образующей обечайки на плоскость продольного сечения обечайки
    /// </summary>
    LocationAccordingToParagraph_5_2_2_4,

    /// <summary>
    /// 5(cil, kon, sfer, torosfer) - наклонный штуцер ось которого лежит в плоскости продольного сечения
    /// </summary>
    LocationAccordingToParagraph_5_2_2_5,

    /// <summary>
    /// 6(oval) - овальное отверстие штуцер перпендикулярно расположен к поверхности обечайки
    /// </summary>
    LocationAccordingToParagraph_5_2_2_6,

    /// <summary>
    /// 7(otbort, torob) - перпендикулярно расположенного к поверхности обечайки или днища штуцера с круглым поперечным сечением при наличии отбортовки или торообразной вставки
    /// </summary>
    LocationAccordingToParagraph_5_2_2_7
}