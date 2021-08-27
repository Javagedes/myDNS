export default class Operation {
    title: string
    action: string
    result_placeholder: string
    fn: () => Promise<string>

    constructor(title: string, action: string, result_placeholder: string, fn: () => Promise<string>)
    {
        this.title = title;
        this.action = action;
        this.result_placeholder = result_placeholder
        this.fn = fn
    }

    async execute() {
        this.result_placeholder = await this.fn()
    }

    

    
}