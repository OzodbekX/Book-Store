import React, {useEffect, useState} from 'react';
import Layout from "../component/Layout";
import {Card, CardContent, FormControl, FormControlLabel, FormLabel, Grid, Radio, RadioGroup} from "@mui/material";
import axiosClient from "../Utils/axios";

const Questions = (props: any) => {
    const [quizes, setQuizes] = useState<TQuiz[]>([])
    useEffect(() => {
        getQuestions()
    }, [])
    const onSubmit = (s: any, item: TQuiz) => {
        if (item.status == "default") {
            let answer = s.currentTarget.defaultValue
            let newQuizes = JSON.parse(JSON.stringify(quizes))
            axiosClient.get(`Questions/CheckAnswer?quizId=${item.questionId}&check=${answer}`)
                .then(res => {
                    if (res.status == 200) {
                        setQuizes(newQuizes.map((q: TQuiz) => {
                            if (q.questionId === item.questionId) {
                                let s: "default" | "true" | "false" = `${res.data}` as "default" | "true" | "false";
                                return {...q, status: s, answer: answer}
                            } else return q
                        }))
                    }
                });
        }
    }
    const getQuestions = () => {
        axiosClient.get('Questions')
            .then(res => {
                if (res?.data?.succeeded) {
                    setQuizes(res.data.data.map((q: TQuiz) => {
                        return {...q, status: "default"}
                    }))
                }
            });
    }

    const changeAnswer = (item: TQuiz, o: string) => {
        if (item.status == "default" || o != item.answer) return ""
        if (item.status == "true") return "text-success"
        if (item.status == "false") return "text-danger"
    }

    const writeAnswers = () => {
        return quizes?.map((q: TQuiz, n: number) => {
            return <Card style={{width: "200px"}} className={"m-1"}>
                <CardContent>
                    <FormControl key={n}>
                        <FormLabel id="demo-radio-buttons-group-label">{q.questionText}</FormLabel>
                        <RadioGroup
                            onChange={(a) => onSubmit(a, q)}
                            aria-labelledby="demo-radio-buttons-group-label"
                            name="radio-buttons-group"
                        >
                            <FormControlLabel className={changeAnswer(q, q.option1)}
                                              value={q.option1}
                                              control={<Radio checked={q.answer == q.option1}/>} label={q.option1}/>
                            <FormControlLabel className={changeAnswer(q, q.option2)}
                                              value={q.option2}
                                              control={<Radio checked={q.answer == q.option2}/>} label={q.option2}/>
                            <FormControlLabel className={changeAnswer(q, q.option3)}
                                              value={q.option3}
                                              control={<Radio checked={q.answer == q.option3}/>} label={q.option3}/>
                        </RadioGroup>
                    </FormControl>
                </CardContent>
            </Card>
        })
    }
    return (
        <Layout title={"Quizes"}>
            <Grid className={"d-flex flex-wrap"}>
                {writeAnswers()}
            </Grid>
        </Layout>
    );
}
type TQuiz =
    {
        answer: "answer"
        option1: "book"
        option2: "notebook"
        option3: "answer"
        questionId: 2
        questionText: "qhat is it"
        quizball: 2
        status: "default" | "true" | "false"
        taskAddress: "giveAndTake,1,1"
    }

export default Questions;