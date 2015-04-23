$(document).ready(function () {
    $("#datepicker1, #datepicker2").datepicker({
        dateFormat: "dd.mm.yy",
        firstDay: 1,
        dayNamesMin: ["Su", "Ma", "Ti", "Ke", "To", "Pe", "La"],
        monthNames: ["Tammikuu", "Helmikuu", "Maaliskuu", "Huhtikuu", "Toukokuu", "Kesäkuu", "Heinäkuu", "Elokuu", "Syyskuu", "Lokakuu", "Marraskuu", "Joulukuu"]
    });
})