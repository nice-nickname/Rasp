(function () {
    $.fn.hasAttr = function (name) {
        return this.attr(name) !== undefined;
    };

    $.fn.dragAndDrop = function (params) {
        const {
            item,
            container,
            ghostClass,
            forCopy,
            dragButton,
            forOne
        } = params

        const INCODING_DRAGGING_ATTR = 'incoding-dragging',
            INCODING_DROP_AREA_ATTR = 'incoding-drop-area',
            INCODING_DRAGGED_ATTR = 'incoding-dragged',
            INCODING_UNIQUE_ATTR = 'incoding-unique-id',
            INCODING_DROPPED_EVENT = 'incodingdropped',
            INCODING_COPY_EVENT = 'incodingcopy',
            INCODING_CANCEL_EVENT = 'incodingdragcancel',
            INCODING_CONTAIN_UNIQUE_EVENT = 'incodingcontainunique',
            INCODING_DRAG_START_EVENT = 'incodingdragstart'

        let elementUnder
        let dropArea
        let ghost = document.createElement('div')
        $(ghost).addClass(ghostClass)

        var zIndex = $($(container)[0]).css('z-index')
        if (+zIndex == 0)
            zIndex = 100

        let overlapElement = document.createElement('div')
        $(overlapElement).attr('role', 'overlap')
            .mouseenter(function () {
                if ($(`[${INCODING_DRAGGING_ATTR}]`).length == 0)
                    $(this).addClass('hidden')
            })
            .attr(INCODING_DROP_AREA_ATTR, '')
            .css({
                'z-index': zIndex,
                position: 'absolute'
            })

        $(container).each(function () {
            if ($(this).prev().attr('role') != 'overlap') {
                let overlapElementCopy = $(overlapElement).clone(true)

                if (forOne && $(this).children().length > 0) 
                    $(overlapElementCopy).attr('role', 'overlap-busy')

                $(overlapElementCopy).insertBefore($(this))
            }
        })

        let raf = null
        let deltaX = 0
        let deltaY = 0
        let startX
        let startY
        let sourceContainer

        $(item).each(function () {
            let item = $(this)
            let sourceContainer = $(this).parent()

            if (dragButton) {
                item.find(dragButton)
                    .off('mousedown')
                    .mousedown(onMouseDown)
            }
            else {
                item.off('mousedown')
                    .mousedown(onMouseDown)
            }


            function onMouseDown(ev) {
                ev.preventDefault()

                if ($(ev.target).is('select')
                    || $(ev.target).is('input')
                    || $(ev.target).is('button')
                    || $(ev.target).is('a.dropdown-item')
                    || $(ev.target).closest('button').length != 0) {
                    return
                }

                if (forCopy) {
                    item = item.clone(true)
                    item.off('mousedown')
                } else {
                    sourceContainer = $(item).parent()
                }

                startX = ev.clientX
                startY = ev.clientY

                let actualElement = $(this)
                if (dragButton) {
                    actualElement = actualElement.closest(item)
                }

                $(item).trigger(INCODING_DRAG_START_EVENT, $(this).data())

                if (forOne) {
                    let overlapBusy = $(item).parent().prev()
                    if (overlapBusy.attr('role') == 'overlap-busy') {
                        overlapBusy
                            .attr('role', 'overlap')
                            .attr(INCODING_DROP_AREA_ATTR, '')
                    }
                }

                item.attr(INCODING_DRAGGING_ATTR, '')
                    .css({
                        position: 'absolute',
                        'z-index': 100000,
                        left: $(actualElement).offset().left,
                        top: $(actualElement).offset().top,
                        width: $(actualElement).width(),
                        height: $(actualElement).height(),
                        'max-width': $(actualElement).width(),
                        'max-height': $(actualElement).height(),
                        cursor: 'grabbing'
                    })


                $('body').append(item)

                let overlap = $('[role=overlap]')

                overlap.removeClass('hidden')
                requestAnimationFrame(() => overlap.addClass('active'))

                $(item).mouseup(onMouseUp)
                document.addEventListener('mousemove', onMouseMove, { passive: true });
            }

            function onMouseMove(ev) {
                if (!raf) {
                    deltaX = ev.clientX - startX
                    deltaY = ev.clientY - startY
                    raf = requestAnimationFrame(moveAt)
                }

                item.css({ 'pointer-events': 'none' })

                elementUnder = $(document.elementFromPoint(ev.clientX, ev.clientY))

                item.css({ 'pointer-events': '' })

                if (elementUnder.length == 0)
                    return

                if (!elementUnder.hasAttr(INCODING_DROP_AREA_ATTR))
                    dropArea = $(elementUnder).closest(INCODING_DROP_AREA_ATTR)
                else
                    dropArea = elementUnder

                if (dropArea.hasClass('hover'))
                    return

                $('[role=overlap].hover').removeClass('hover')
                dropArea.addClass('hover')
            }

            function moveAt() {
                if (!item.hasAttr(INCODING_DRAGGING_ATTR))
                    return

                item.css({ transform: `translate3d(${deltaX}px, ${deltaY}px, 0px)` })
                raf = null
            }

            function onMouseUp(ev) {
                ev.preventDefault()

                if (raf)
                    cancelAnimationFrame(raf = null)

                item.removeAttr(INCODING_DRAGGING_ATTR)
                    .removeAttr('style')

                let isDropArea = dropArea.length > 0

                if (isDropArea) {
                    item.attr(INCODING_DRAGGED_ATTR, '')

                    let dropContainer = dropArea.next()
                    let containUnique = false

                    if (dropContainer.find(`[${INCODING_UNIQUE_ATTR}=${item.attr(INCODING_UNIQUE_ATTR)}]`).length > 0) {
                        dropContainer.trigger(INCODING_CONTAIN_UNIQUE_EVENT, item.data())
                        containUnique = true
                        item.remove()
                    }

                    if (!containUnique) {
                        let event = forCopy ? INCODING_COPY_EVENT : INCODING_DROPPED_EVENT

                        let itemUnderDrop = $(elementUnder).closest(params.item)
                        if (itemUnderDrop.length != 0) {
                            $(itemUnderDrop).before(item)
                            console.log(1)
                        }
                        else {
                            $(item).appendTo(dropContainer)
                            console.log(2)
                        }
                        dropContainer.trigger(event, $(this).data())
                    }
                } else {
                    if (forCopy) {
                        item.remove()
                    } else if (forOne) {
                        $(sourceContainer).append(item)
                        let prev = $(sourceContainer).prev()
                        if (prev.hasAttr('role', 'overlap'))
                            prev.attr('role', 'overlap-busy')
                    } else {
                        let itemUnderDrop = $(document.elementFromPoint(ev.clientX, ev.clientY)).closest(params.item)
                        if (itemUnderDrop.length != 0) {
                            $(itemUnderDrop).before(item)
                        } else {
                            $(item).appendTo(sourceContainer)
                        }
                    }
                }

                if (forOne) {
                    $('[role=overlap].active.hover')
                        .removeClass('active hover')
                        .attr('role', 'overlap-busy')
                        .removeAttr(INCODING_DROP_AREA_ATTR)
                }

                $('[role=overlap]').removeClass('active hover')

                item.off('mouseup')
                document.removeEventListener('mousemove', onMouseMove)
            }
        })
    }
}(jQuery));
