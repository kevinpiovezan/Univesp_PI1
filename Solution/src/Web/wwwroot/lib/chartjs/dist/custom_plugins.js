/*CUSTOM PLUGINS PRA LIB*/


/* 1 - Plugin pra permitir a tooltip aparecer embaixo do gráfico
 * ao invés de ficar ao lado do mouse e obstruir o conteúdo que poderia estar no meio do doughnut*/

const tooltipPlugin = Chart.registry.getPlugin('tooltip');
tooltipPlugin.positioners.bottom = function (elements, eventPosition) {
    /** @@type {Tooltip} */
    var tooltip = this;
    /* ... */

    return {
        x: eventPosition.x,
        y: eventPosition.y + 1000 // valor grande arbitrário
    };
};